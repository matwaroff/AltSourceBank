import { Component } from '@angular/core';
import { AuthenticationService } from '../services/authentication.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  constructor(
    private authService: AuthenticationService,
    private router: Router
  ) { };
  isExpanded = false;

  collapse() {
    this.isExpanded = false;
  }

  signedIn() {
    return this.authService.signedIn;
  }

  logout() {
    this.authService.logout();
    this.router.navigate(["/"]);
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }
}
