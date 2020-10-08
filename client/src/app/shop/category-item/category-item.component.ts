import { Component, Input, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasketItem } from 'src/app/shared/models/basket';
import { ICategory } from 'src/app/shared/models/category';

@Component({
  selector: 'app-category-item',
  templateUrl: './category-item.component.html',
  styleUrls: ['./category-item.component.scss']
})
export class CategoryItemComponent implements OnInit {
  @Input() category: ICategory;

  constructor(private basketService: BasketService) { }

  ngOnInit(): void {
  }

  addItemToBasket(): any {
    // this.basketService.addItemToBasket(this.category, 1, false, 24, '', 0, 0, false);
    this.basketService.addItemToBasket(this.category, 1);
  }
}
