import { HttpInterceptorFn, HttpErrorResponse } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ToastService } from './toast.service';

function getMessage(status: number): string {
  switch (status) {
    case 0:
      return 'Sem conexão. Verifique sua rede.';
    case 400:
      return 'Dados inválidos. Verifique e tente novamente.';
    case 401:
      return 'Não autorizado. Faça login novamente.';
    case 403:
      return 'Você não tem permissão para esta ação.';
    case 404:
      return 'Recurso não encontrado.';
    case 409:
      return 'Conflito: o recurso já existe ou foi alterado.';
    case 422:
      return 'Dados inválidos. Verifique os campos.';
    case 500:
    default:
      return 'Erro no servidor. Tente novamente mais tarde.';
  }
}

export const httpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);

  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      const skipToast = req.headers.get('X-Skip-Error-Toast') === 'true';
      if (!skipToast) {
        const message = err.error?.message && typeof err.error.message === 'string'
          ? err.error.message
          : getMessage(err.status || 0);
        toast.error(message);
      }
      return throwError(() => err);
    }),
  );
};
