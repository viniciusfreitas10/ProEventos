import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map, take } from 'rxjs/operators';
import { User } from '../models/Identity/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  baseUrl = environment.apiURL + 'api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any) : Observable<void>
  {
    return this.http.post<User>(this.baseUrl +  'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if(user){

        }
      })
    );
  }
}
