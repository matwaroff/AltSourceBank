import { Component, OnInit } from '@angular/core';
import { Account } from '../models/account';
import { AccountService } from '../services/account.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { first, flatMap } from 'rxjs/operators';
import { AlertService } from '../services/alert.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit {
  public account: Account;
  public depositForm: FormGroup;
  public withdrawlForm: FormGroup;
  loading: boolean = false;

  constructor(
    private accountService: AccountService,
    private formBuilder: FormBuilder,
    private alertService: AlertService
  ) {
    this.account = this.accountService.account;
  }

  ngOnInit() {
    this.depositForm = this.formBuilder.group({
      depositAmount: ['', Validators.requiredTrue]
    });
    this.withdrawlForm = this.formBuilder.group({
      withdrawlAmount: ['', Validators.requiredTrue],
    });
  }

  depositSubmit() {
    let _t = this;
    this.loading = true;
    if (["", 0, "0"].includes(this.depositForm.controls.depositAmount.value)) {
      return;
    }
    this.accountService.deposit(this.depositForm.controls.depositAmount.value || 0, this.account.authToken).pipe(
      flatMap(newBalance => _t.accountService.updateAccount(_t.account.authToken))
    ).subscribe(updatedAccount => {
      //reload acct
      _t.account = Account.fromJSON(updatedAccount);
      _t.loading = false;
      sessionStorage.altSourceAcct = JSON.stringify(updatedAccount);
    },
    err => {
      console.log(err);
    })
  }

  withdrawlSubmit() {
    let _t = this;
    this.loading = true;
    if (["", 0, "0"].includes(this.withdrawlForm.controls.withdrawlAmount.value)) {
      return;
    }
    this.accountService.withdrawl(this.withdrawlForm.controls.withdrawlAmount.value || 0, this.account.authToken).pipe(
      flatMap(newBalance => {
        if (newBalance.hasOwnProperty("error")) {
          _t.alertService.error(newBalance["error"]);
        }
        return _t.accountService.updateAccount(_t.account.authToken)
      })
    ).subscribe(updatedAccount => {
      //reload acct
      _t.account = Account.fromJSON(updatedAccount);
      _t.loading = false;
      sessionStorage.altSourceAcct = JSON.stringify(updatedAccount);
    },
    err => {
      console.log(err);
    })
  }

}
