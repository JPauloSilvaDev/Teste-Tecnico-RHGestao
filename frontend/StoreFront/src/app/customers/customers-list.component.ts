import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { CustomersService } from '../services/customers.service';
import { Customer } from '../models/customer.model';
import { ToastService } from '../core/toast.service';
import { ConfirmService } from '../core/confirm.service';

@Component({
  selector: 'app-customers-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './customers-list.component.html',
  styleUrl: './customers-list.component.css',
})
export class CustomersListComponent implements OnInit {
  private customersService = inject(CustomersService);
  private toastService = inject(ToastService);
  private confirmService = inject(ConfirmService);

  customers: Customer[] = [];
  loading = false;
  error = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.error = false;
    this.loading = true;
    this.customersService.getAll().subscribe({
      next: (list) => {
        this.customers = list;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.error = true;
      },
    });
  }

  deleteCustomer(customer: Customer, event: Event): void {
    event.preventDefault();
    if (!customer.id) return;
    const name = customer.name || 'este cliente';
    this.confirmService.confirm({
      title: 'Excluir cliente',
      message: `Excluir "${name}"? Esta ação não pode ser desfeita.`,
      confirmLabel: 'Excluir',
    }).subscribe((confirmed) => {
      if (!confirmed) return;
      this.customersService.delete(customer.id!).subscribe({
        next: () => {
          this.customers = this.customers.filter((c) => c.id !== customer.id);
          this.toastService.success('Cliente excluído.');
        },
      });
    });
  }
}
