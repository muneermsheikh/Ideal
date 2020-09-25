import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { CategoryItemComponent } from './category-item/category-item.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    ShopComponent,
    CategoryItemComponent
  ],

  imports: [
    CommonModule,
    SharedModule,
  ]
})

export class ShopModule { }
