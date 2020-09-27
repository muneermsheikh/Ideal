import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShopComponent } from './shop.component';
import { CategoryItemComponent } from './category-item/category-item.component';
import { SharedModule } from '../shared/shared.module';
import { CategoryDetailsComponent } from './category-details/category-details.component';
// import { RouterModule } from '@angular/router'; -- not reqd after lazy loading
import { ShopRoutingModule } from './shop-routing.module';

@NgModule({
  declarations: [
    ShopComponent,
    CategoryItemComponent,
    CategoryDetailsComponent
  ],

  imports: [
    CommonModule,
    SharedModule,
    ShopRoutingModule
  ],
  // exports: [ ShopComponent]  -- not required after lazy loading

})

export class ShopModule { }
