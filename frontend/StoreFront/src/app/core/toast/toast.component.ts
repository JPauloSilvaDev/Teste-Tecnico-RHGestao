import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, Toast } from '../toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css',
})
export class ToastComponent implements OnInit, OnDestroy {
  private toastService = inject(ToastService);
  toasts: Toast[] = [];
  private unsub: (() => void) | null = null;

  ngOnInit(): void {
    this.unsub = this.toastService.subscribe((list) => (this.toasts = list));
  }

  ngOnDestroy(): void {
    this.unsub?.();
  }

  dismiss(id: number): void {
    this.toastService.dismiss(id);
  }
}
