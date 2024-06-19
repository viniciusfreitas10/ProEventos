import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../services/Account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private router: Router, private accountService: AccountService) { }
  isCollapsed = true;
  ngOnInit() {
  }

  showMenu(): boolean {
    return this.router.url !== '/user/login';
  }

  public Logout(): void{
    console.log("entrei logout")
    this.accountService.logout();
  }
}
