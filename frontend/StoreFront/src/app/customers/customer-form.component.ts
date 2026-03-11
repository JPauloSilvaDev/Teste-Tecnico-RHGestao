import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { CustomersService } from '../services/customers.service';

@Component({
  selector: 'app-customer-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './customer-form.component.html',
  styleUrl: './customer-form.component.css',
})
export class CustomerFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private customersService = inject(CustomersService);

  isEdit = false;
  customerId: string | null = null;
  submitting = false;

  form = this.fb.nonNullable.group({
    name: ['', [Validators.required, Validators.maxLength(150)]],
    email: ['', [Validators.required, Validators.email, Validators.maxLength(100)]],
  });

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.customerId = id;
      this.customersService.getById(id).subscribe({
        next: (c) => {
          this.form.patchValue({ name: c.name, email: c.email });
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
    if (this.isEdit && this.customerId) {
      this.customersService.update({ ...value, id: this.customerId }).subscribe({
        next: () => this.router.navigate(['/customers']),
        error: () => { this.submitting = false; },
      });
    } else {
      this.customersService.create(value).subscribe({
        next: () => this.router.navigate(['/customers']),
        error: () => { this.submitting = false; },
      });
    }
  }
}
