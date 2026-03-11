import { Routes } from '@angular/router';
import { ProductsListComponent } from './products/products-list.component';
import { ProductFormComponent } from './products/product-form.component';
import { CustomersListComponent } from './customers/customers-list.component';
import { CustomerFormComponent } from './customers/customer-form.component';
import { OrdersListComponent } from './orders/orders-list.component';
import { OrderCreateComponent } from './orders/order-create.component';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'products' },
  { path: 'products', component: ProductsListComponent },
  { path: 'products/new', component: ProductFormComponent },
  { path: 'products/:id/edit', component: ProductFormComponent },
  { path: 'customers', component: CustomersListComponent },
  { path: 'customers/new', component: CustomerFormComponent },
  { path: 'customers/:id/edit', component: CustomerFormComponent },
  { path: 'orders', component: OrdersListComponent },
  { path: 'orders/new', component: OrderCreateComponent },
  { path: '**', redirectTo: 'products' },
];
