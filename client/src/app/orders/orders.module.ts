import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { OrderCreateComponent } from './order-create/order-create.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { SharedModule } from '../shared/shared.module';
import { OrderEditComponent } from './order-edit/order-edit.component';



@NgModule({
  declarations: [OrdersComponent, OrderCreateComponent, OrderEditComponent],
  imports: [
    CommonModule,
    SharedModule,
    OrdersRoutingModule
  ],
  exports: [
  ]
})
export class OrdersModule { }
