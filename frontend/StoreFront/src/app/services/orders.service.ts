import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateOrderDto, OrderResponse } from '../models/order.model';
import { injectApiBaseUrl } from '../core/api-url.token';

@Injectable({
  providedIn: 'root',
})
export class OrdersService {
  private http = inject(HttpClient);
  private readonly baseUrl = `${injectApiBaseUrl()}/orders`;

  getAll(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(this.baseUrl);
  }

  getById(id: string): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.baseUrl}/${id}`);
  }

  create(dto: CreateOrderDto): Observable<OrderResponse> {
    return this.http.post<OrderResponse>(this.baseUrl, dto);
  }
}

