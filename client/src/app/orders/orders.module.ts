import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { OrderCreateComponent } from './order-create/order-create.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [OrdersComponent, OrderCreateComponent],
  imports: [
    CommonModule,
    SharedModule,
    OrdersRoutingModule
  ],
  exports: [
  ]
})
export class OrdersModule { }
