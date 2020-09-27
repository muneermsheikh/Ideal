import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ICategory } from 'src/app/shared/models/category';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-category-details',
  templateUrl: './category-details.component.html',
  styleUrls: ['./category-details.component.scss']
})
export class CategoryDetailsComponent implements OnInit {
  category: ICategory;

  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadCategory();
  }

  loadCategory() {
    return this.shopService.getCategory(+this.activatedRoute.snapshot.paramMap.get('id')).subscribe( category => {
      this.category = category;
    }, error => {
      console.log(error);
    }
    );
  }

}
