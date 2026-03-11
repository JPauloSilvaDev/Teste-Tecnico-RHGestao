import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface ConfirmOptions {
  title?: string;
  message: string;
  confirmLabel?: string;
  cancelLabel?: string;
}

@Injectable({
  providedIn: 'root',
})
export class ConfirmService {
  private currentResolve: ((value: boolean) => void) | null = null;
  private _options: ConfirmOptions | null = null;

  get isOpen(): boolean {
    return this._options !== null;
  }

  get options(): ConfirmOptions | null {
    return this._options;
  }

  confirm(options: ConfirmOptions | string): Observable<boolean> {
    const opts: ConfirmOptions = typeof options === 'string'
      ? { message: options }
      : options;
    this._options = {
      title: opts.title ?? 'Confirmar',
      message: opts.message,
      confirmLabel: opts.confirmLabel ?? 'Confirmar',
      cancelLabel: opts.cancelLabel ?? 'Cancelar',
    };
    return new Observable<boolean>((observer) => {
      this.currentResolve = (value) => {
        observer.next(value);
        observer.complete();
      };
      return () => {
        this.currentResolve = null;
      };
    });
  }

  choose(value: boolean): void {
    if (this.currentResolve) {
      this.currentResolve(value);
      this.currentResolve = null;
      this._options = null;
    }
  }
}
