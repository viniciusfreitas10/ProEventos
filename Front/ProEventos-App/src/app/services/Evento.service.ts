import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';
import { take } from 'rxjs/operators'
import { environment } from 'src/environments/environment';

@Injectable()

export class EventoService {
  token = new HttpHeaders({
    'Authorization' : `Bearer ${JSON.parse(localStorage.getItem('user')).token}`
  });

  baseURL = environment.apiURL + 'api/Evento/';
  constructor(private http: HttpClient) {

  }

  public getEventos(): Observable<Evento[]>{
    console.log("token : " + JSON.parse(localStorage.getItem('user')).token)
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
    console.log("token : " + JSON.parse(localStorage.getItem('user')).token)
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
