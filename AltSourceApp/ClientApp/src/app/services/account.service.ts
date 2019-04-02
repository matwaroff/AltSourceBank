import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Account } from '../models/account';

@Injectable()
export class AccountService {
  baseUrl: string;
  constructor(private http: HttpClient) {
    this.baseUrl = '';
  }

  get account() {
    return Account.fromJSON(sessionStorage.altSourceAcct);
  }

  balance(authToken: string) {
    return this.http.put(`${this.baseUrl}/accounts/balance`, { authToken: authToken });
  }

  register(user: Account) {
    return this.http.post(`${this.baseUrl}/accounts/register`, user);
  }

  updateAccount(authToken: string) {
    return this.http.put(`${this.baseUrl}/accounts/update`, { authToken })
  }

  deposit(amount: number, authToken: string) {
    return this.http.post(`${this.baseUrl}/accounts/deposit`, { amount, authToken })
  }
  withdrawl(amount: number, authToken: string) {
    return this.http.post(`${this.baseUrl}/accounts/withdrawl`, {amount, authToken })
  }
  delete(id: number) {
    return this.http.delete(`${this.baseUrl}/users/` + id);
  }
}
