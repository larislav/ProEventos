import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {
baseURL = environment.apiURL + 'api/redesSociais'

constructor(private http: HttpClient) { }

/**
 *
 * @param origem Informar rota 'palestrante' ou 'evento'
 * @param id Informar PalestranteId ou EventoId
 * @returns Observable<RedeSocial[]>
 */
public getRedesSociais(origem: string, id: number): Observable<RedeSocial[]> {
  let URL =
    id === 0 ? `${this.baseURL}/${origem}`
    : `${this.baseURL}/${origem}/${id}`
  return this.http.get<RedeSocial[]>(URL).pipe(take(1))
}

/**
 *
 * @param origem Informar rota 'palestrante' ou 'evento'
 * @param id Informar PalestranteId ou EventoId
 * @param redesSociais Informar Redes Sociais em array
 * @returns Observable<RedeSocial[]>
 */
 public saveRedesSociais(origem: string, id: number, redesSociais: RedeSocial[]): Observable<RedeSocial[]> {
  let URL =
    id === 0 ? `${this.baseURL}/${origem}`
    : `${this.baseURL}/${origem}/${id}`
  return this.http.put<RedeSocial[]>(URL, redesSociais).pipe(take(1))
}

/**
 *
 * @param origem Informar rota 'palestrante' ou 'evento'
 * @param id Informar PalestranteId ou EventoId
 * @param redeSocialId Informar o id da rede social
 * @returns Observable<any>
 */
 public deleteRedeSocial(origem: string, id: number, redeSocialId: number): Observable<any> {
  let URL =
    id === 0 ? `${this.baseURL}/${origem}/${redeSocialId}`
    : `${this.baseURL}/${origem}/${id}/${redeSocialId}`
  return this.http.delete(URL).pipe(take(1))
}

}
