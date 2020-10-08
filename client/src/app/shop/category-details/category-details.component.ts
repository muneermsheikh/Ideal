import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { ICategory } from 'src/app/shared/models/category';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {
  category: ICategory;
  quantity = 1;

  constructor(private shopService: ShopService,
              private basketService: BasketService,
              private activatedRoute: ActivatedRoute,
              private bcService: BreadcrumbService) {
    this.bcService.set('@categoryname', '');
   }

  ngOnInit(): void {
    this.loadCategory();
  }

  addItemToBasket(): void {
    console.log(this.category);
    // this.basketService.addItemToBasket(this.category, this.quantity, false, 24, '', 0, 0, false);
    this.basketService.addItemToBasket(this.category, this.quantity);
  }

  incrementQuantity(): void {
    this.quantity++;
  }

  decrementQuantity(): void {
    if (this.quantity > 1) {
      this.quantity--;
    }
  }

  loadCategory(): void {
    this.shopService.getCategory(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe( category => {
      this.category = category;
      this.bcService.set('@categoryname', category.name);
    }, error => {
      console.log(error);
    });
  }


}
