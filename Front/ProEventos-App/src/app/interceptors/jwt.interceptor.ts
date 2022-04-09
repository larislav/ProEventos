import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/account.service';
import { catchError, take } from 'rxjs/operators';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {
//Classe que herda o HttpInterceptor
//e vai receber qualquer requisição que vc fizer
//vai clonar a requisição, e vai manipular o header
  constructor(private accountService: AccountService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    let currentUser: User;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => {
      currentUser = user
      if(currentUser) {
        request = request.clone(
          {
            setHeaders: {
              Authorization: `Bearer ${currentUser.token}`
            }
          }
        );
      }
    });


    return next.handle(request).pipe(
      catchError(error => {
        if(error){
          localStorage.removeItem('user');
        }
        return throwError(error);
      })
    );
  }
}
