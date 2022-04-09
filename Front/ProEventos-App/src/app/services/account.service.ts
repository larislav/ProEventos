import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/identity/User';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(5);
  //buffer de 1
  //variavel que vai receber diversas atualizacoes, pq toda vez
  //que eu logar, toda vez que eu atualizar o registro do meu usuario,
  //ou seja, toda vez que o token for atualizado, voce tem que passar
  //para as partes do sistema que o token foi alterado
  public currentUser$ = this.currentUserSource.asObservable();
  //toda vez que cria uma variável que é um Observable ou um Subject,
  //geralmente coloca o $

  baseUrl = environment.apiURL + 'api/account/'

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void>{
    return this.http.post<User>(this.baseUrl + 'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if (user){
          this.setCurrentUser(user)
        }
      }) // o map vai pegar o retorno do POST
      );
    }

    getUser(): Observable<UserUpdate> {
      return this.http.get<UserUpdate>(this.baseUrl + 'getUser').pipe(take(1));
    }

    updateUser(model: UserUpdate): Observable<void>{
      return this.http.put<UserUpdate>(this.baseUrl + 'UpdateUser', model)
      .pipe(take(1),
      map((user: UserUpdate) =>
      {
        this.setCurrentUser(user);
      }
      )
      )
    }

    public register(model: any): Observable<void>{
      return this.http.post<User>(this.baseUrl + 'register', model).pipe(
        take(1),
        map((response: User) => {
          const user = response;
          if (user){
            this.setCurrentUser(user)
          }
        })
        );
      }

      logout(): void{
        localStorage.removeItem('user');
        this.currentUserSource.next(null);
        this.currentUserSource.complete();
      }

      public setCurrentUser(user: User): void{
        localStorage.setItem('user', JSON.stringify(user));
        //Observable serve para que eu não precise ficar chamando o localstorage o tempo todo

        this.currentUserSource.next(user);
      }

      public postUpload(file: File): Observable<UserUpdate>{
        const fileToUpload = file[0] as File;
        const formData = new FormData();
        formData.append('file', fileToUpload);
        return this.http.post<UserUpdate>(`${this.baseUrl}upload-image`, formData)
        .pipe(take(1));
      }

    }
