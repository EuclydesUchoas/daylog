import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { MessageService, ToastMessageOptions } from 'primeng/api';
import { tap, throwError } from 'rxjs';

export const globalHttpErrorInterceptor: HttpInterceptorFn = (req, next) => {
  const messageService = inject(MessageService);

  function getToastMessageOptionsByError(error: HttpErrorResponse): ToastMessageOptions {
    switch (error.error?.error?.type) {
      case 1: return { 
        severity: 'warn',
        summary: 'Aviso',
        detail: error.error.error.description || error.message,
      };
      case 2: return {
        severity: 'warn',
        summary: 'Falha',
        detail: error.error.error.description || error.message,
      };
      default: return {
        severity: 'error',
        summary: 'Erro',
        detail: error.error?.error?.description || error.message,
      };
    }
  }

  return next(req).pipe(
    tap({
      error: (error: HttpErrorResponse) => {
        messageService.add(getToastMessageOptionsByError(error));
        // return throwError(() => err);
      }
    })
  );
};
