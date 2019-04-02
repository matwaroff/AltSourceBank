import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { LoginRequest, Account } from '../models/account';
import { map } from 'rxjs/operators';
@Injectable()
export class AuthenticationService {
  constructor(private http: HttpClient) {}

  login(username: string, password: string) {
    let _t = this;
    return this.http.put<any>('/accounts/login', { username: username, password: password }, {headers: new HttpHeaders({'Content-Type': 'application/json'}) })
      .pipe(map(user => {
        if (user && user.authToken) {
          sessionStorage.setItem('altSourceAcct', JSON.stringify(user));
        }
        return user;
      })
    )
  }

  get signedIn() {
    return sessionStorage.getItem("altSourceAcct") != null;
  }
  logout() {
    sessionStorage.removeItem("altSourceAcct");
  }
}
