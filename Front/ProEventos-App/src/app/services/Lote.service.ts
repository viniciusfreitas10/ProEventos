import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Lote } from '../models/Lote';
import { take } from 'rxjs/operators'


@Injectable()

export class LoteService {
  baseURL = 'https://localhost:44352/api/Lotes/';
  constructor(private http: HttpClient) { }

  public GetLotesByEventId(eventoId: number ): Observable<Lote[]>{
    return this.http
    .get<Lote[]>(`${this.baseURL}${eventoId}`)
    .pipe(take(1))
    ;
  }
  public SaveLote(eventoId: number, models: Lote[]): Observable<Lote[]>{
    return this.http
    .put<Lote[]>(`${this.baseURL}${eventoId}`, models)
    .pipe(take(1));
  }
  public DeleteLote(eventoId: number, loteId: number): Observable<any>{
    return this.http
    .delete(`${this.baseURL}${eventoId}/${loteId}`)
    .pipe(take(1));
  }
}
