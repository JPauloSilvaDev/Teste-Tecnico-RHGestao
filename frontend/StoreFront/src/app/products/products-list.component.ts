import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ProductsService } from '../services/products.service';
import { Product } from '../models/product.model';
import { ToastService } from '../core/toast.service';
import { ConfirmService } from '../core/confirm.service';

@Component({
  selector: 'app-products-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './products-list.component.html',
  styleUrl: './products-list.component.css',
})
export class ProductsListComponent implements OnInit {
  private productsService = inject(ProductsService);
  private toastService = inject(ToastService);
  private confirmService = inject(ConfirmService);

  products: Product[] = [];
  loading = false;
  error = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.error = false;
    this.loading = true;
    this.productsService.getAll().subscribe({
      next: (list) => {
        this.products = list;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.error = true;
      },
    });
  }

  deleteProduct(product: Product, event: Event): void {
    event.preventDefault();
    if (!product.id) return;
    const name = product.name || 'este produto';
    this.confirmService.confirm({
      title: 'Excluir produto',
      message: `Excluir "${name}"? Esta ação não pode ser desfeita.`,
      confirmLabel: 'Excluir',
    }).subscribe((confirmed) => {
      if (!confirmed) return;
      this.productsService.delete(product.id!).subscribe({
        next: () => {
          this.products = this.products.filter((p) => p.id !== product.id);
          this.toastService.success('Produto excluído.');
        },
      });
    });
  }
}
