import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import {
  FormArray,
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CustomersService } from '../services/customers.service';
import { ProductsService } from '../services/products.service';
import { OrdersService } from '../services/orders.service';
import { Customer } from '../models/customer.model';
import { Product } from '../models/product.model';

@Component({
  selector: 'app-order-create',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './order-create.component.html',
  styleUrl: './order-create.component.css',
})
export class OrderCreateComponent implements OnInit {
  form: FormGroup;
  customers: Customer[] = [];
  products: Product[] = [];
  submitting = false;

  get items(): FormArray {
    return this.form.get('items') as FormArray;
  }

   get total(): number {
    return this.items.controls.reduce((sum, control) => {
      const productId = control.get('productId')?.value as string | null;
      const quantity = Number(control.get('quantity')?.value) || 0;
      const product = this.getProduct(productId);
      if (!product) {
        return sum;
      }
      return sum + product.price * quantity;
    }, 0);
  }

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private customersService: CustomersService,
    private productsService: ProductsService,
    private ordersService: OrdersService,
  ) {
    this.form = this.fb.group({
      clientId: ['', Validators.required],
      items: this.fb.array([]),
    });
  }

  ngOnInit(): void {
    this.customersService.getAll().subscribe((customers) => (this.customers = customers));
    this.productsService.getAll().subscribe((products) => (this.products = products));
    this.addItem();
  }

  addItem(): void {
    this.items.push(
      this.fb.group({
        productId: ['', Validators.required],
        quantity: [1, [Validators.required, Validators.min(1)]],
      }),
    );
  }

  removeItem(index: number): void {
    if (this.items.length > 1) {
      this.items.removeAt(index);
    }
  }

  getProduct(id: string | null | undefined): Product | undefined {
    if (!id) {
      return undefined;
    }
    return this.products.find((p) => p.id === id);
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }
    this.submitting = true;
    this.ordersService.create(this.form.value).subscribe({
      next: () => this.router.navigate(['/orders']),
      error: () => { this.submitting = false; },
    });
  }
}

