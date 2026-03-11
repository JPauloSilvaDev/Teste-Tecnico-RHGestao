import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { OrdersService } from '../services/orders.service';
import { OrderResponse } from '../models/order.model';

@Component({
  selector: 'app-orders-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './orders-list.component.html',
  styleUrl: './orders-list.component.css',
})
export class OrdersListComponent implements OnInit {
  private ordersService = inject(OrdersService);

  orders: OrderResponse[] = [];
  loading = false;
  error = false;

  ngOnInit(): void {
    this.loadData();
  }

  loadData(): void {
    this.error = false;
    this.loading = true;
    this.ordersService.getAll().subscribe({
      next: (orders) => {
        this.orders = orders;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
        this.error = true;
      },
    });
  }
}

