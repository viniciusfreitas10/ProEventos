import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { take } from 'rxjs/operators'
import { environment } from 'src/environments/environment';

@Injectable()

export class EventoService {
  token = new HttpHeaders({
    'Authorization' : 'Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIzIiwidW5pcXVlX25hbWUiOiJ2aW5pIiwibmJmIjoxNzE4NzU1ODczLCJleHAiOjE3MTg3NTk0NzMsImlhdCI6MTcxODc1NTg3M30.N74z9lrSDZp3NZZ-qBlJe9UTvGZMNvn9HytCklbljlUKKohLtOGy2HuM3YMu7EQt--0uc6cfOLhRS-Jx3ZFTpw'
  });
  baseURL = environment.apiURL + 'api/Evento/';
  constructor(private http: HttpClient) {
    //Headers.apply.arguments = "bearer 83489fj4389fj3894fj98fj894fj9"
  }

  public getEventos(): Observable<Evento[]>{
    return this.http.get<Evento[]>(this.baseURL, { headers: this.token})
    .pipe(take(1));
  }

  public getElemntosByTema(tema: string): Observable<Evento[]>{
    return this.http.get<Evento[]>(`${this.baseURL}${tema}/tema`, { headers: this.token})
    .pipe(take(1));
  }

  public getEventoById(id: number): Observable<Evento>{
    return this.http.get<Evento>(`${this.baseURL}${id}`, { headers: this.token})
    .pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento>{
    return this.http
    .post<Evento>(this.baseURL, evento, { headers: this.token})
    .pipe(take(1));
  }
  public put(evento: Evento): Observable<Evento>{
    return this.http
    .put<Evento>(`${this.baseURL}${evento.id}`, evento, { headers: this.token})
    .pipe(take(1));
  }
  public DeleteEvento(id: number): Observable<any>{
    return this.http
    .delete(`${this.baseURL}${id}`, { headers: this.token})
    .pipe(take(1));
  }
  public postUpload(eventoId: number, file: File): Observable<Evento>{
    const fileToUpload = file[0] as File;
    const formData = new FormData();

    formData.append('file',fileToUpload)

    return this.http
    .post<Evento>(`${this.baseURL}upload-image/${eventoId}`, formData, { headers: this.token})
    .pipe(take(1));
  }
}
