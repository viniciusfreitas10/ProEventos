import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { map, take } from 'rxjs/operators';
import { User } from '../models/Identity/User';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  baseUrl = environment.apiURL + 'api/account/';

  constructor(private http: HttpClient) { }

  public login(model: any) : Observable<void>{
    return this.http.post<User>(this.baseUrl +  'login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;
        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }

  public logout(): void{
    console.log("entrei logout account")
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
    this.currentUserSource.complete();
  }

  public setCurrentUser(User: User): void{
    localStorage.setItem('user', JSON.stringify(User));
    this.currentUserSource.next(User);
  }
}
