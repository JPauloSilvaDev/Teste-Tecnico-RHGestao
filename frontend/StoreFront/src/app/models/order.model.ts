export interface OrderItemResponse {
  productId: string;
  productName: string;
  quantity: number;
  unitPrice: number;
  subtotal: number;
}

export interface OrderResponse {
  id: string;
  clientName: string;
  orderDate: string;
  items: OrderItemResponse[];
  totalValue: number;
}

export interface CreateOrderItemDto {
  productId: string;
  quantity: number;
}

export interface CreateOrderDto {
  clientId: string;
  items: CreateOrderItemDto[];
}

