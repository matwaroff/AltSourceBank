import { Transaction } from "./transaction";
import { PACKAGE_ROOT_URL } from "@angular/core";

export class Account {
  constructor(
    public id: string = null,
    public creationDate: Date = null,
    public username: string = null,
    public authToken: string = null,
    public balance: number = null,
    public firstName: string = null,
    public lastName: string = null,
    public transactionHistory: Transaction[] = null
  ) { }

  static fromJSON(json: any): Account {
    if (json.constructor == String) {
      return Object.assign(new Account(), JSON.parse(json));
    } else if (json.constructor == Object) {
      return Object.assign(new Account(), json);
    }
    
  }

}

export class LoginRequest {
  constructor(
    public username: string,
    public password: string
  ) { }
}

export class RegisterRequest {
  constructor(
    public firstName,
    public lastName,
    public username,
    public password
  ) { }
}
