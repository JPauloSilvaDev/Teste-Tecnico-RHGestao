import { Injectable } from '@angular/core';

export type ToastType = 'success' | 'error' | 'info';

export interface Toast {
  id: number;
  message: string;
  type: ToastType;
}

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  private nextId = 0;
  private toasts: Toast[] = [];
  private listeners: ((toasts: Toast[]) => void)[] = [];

  get snapshot(): Toast[] {
    return [...this.toasts];
  }

  subscribe(listener: (toasts: Toast[]) => void): () => void {
    this.listeners.push(listener);
    listener(this.snapshot);
    return () => {
      this.listeners = this.listeners.filter((l) => l !== listener);
    };
  }

  private emit(): void {
    const snapshot = this.snapshot;
    this.listeners.forEach((l) => l(snapshot));
  }

  show(message: string, type: ToastType = 'info'): void {
    const id = this.nextId++;
    this.toasts.push({ id, message, type });
    this.emit();
    setTimeout(() => this.dismiss(id), 4000);
  }

  success(message: string): void {
    this.show(message, 'success');
  }

  error(message: string): void {
    this.show(message, 'error');
  }

  dismiss(id: number): void {
    this.toasts = this.toasts.filter((t) => t.id !== id);
    this.emit();
  }
}
