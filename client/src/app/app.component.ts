import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {
  title = 'client';

  constructor(private basketService: BasketService, private accountService: AccountService) {}


  // initialize all application level values here
  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadCurrentBasket();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe(() => {
      }, error => {
        console.log(error);
      });
  }

  loadCurrentBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId)
    {
      this.basketService.getBasket(basketId).subscribe(() =>
      {
        console.log('initialized basket');
      }, error => {
        console.log(error);
      });
    }
  }

}
