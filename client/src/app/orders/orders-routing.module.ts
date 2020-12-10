import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [],
  imports: [
    RouterModule,
    CommonModule,
    SharedModule
  ],
  exports: [
    RouterModule
  ]
})
export class OrdersRoutingModule { }