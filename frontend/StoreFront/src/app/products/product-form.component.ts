import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { ProductsService } from '../services/products.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css',
})
export class ProductFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private productsService = inject(ProductsService);

  isEdit = false;
  productId: string | null = null;
  submitting = false;

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.maxLength(500)]],
    price: [0, [Validators.required, Validators.min(0)]],
    quantityInStock: [0, [Validators.required, Validators.min(0)]],
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.productId = id;
      this.productsService.getById(id).subscribe({
        next: (p) => {
          this.form.patchValue({
            name: p.name,
            description: p.description ?? '',
            price: p.price,
            quantityInStock: p.quantityInStock ?? 0,
          });
        },
      });
    }
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting = true;
    const value = this.form.getRawValue();
    if (this.isEdit && this.productId) {
      this.productsService.update({ ...value, id: this.productId }).subscribe({
        next: () => this.router.navigate(['/products']),
        error: () => { this.submitting = false; },
      });
    } else {
      this.productsService.create(value).subscribe({
        next: () => this.router.navigate(['/products']),
        error: () => { this.submitting = false; },
      });
    }
  }
}
