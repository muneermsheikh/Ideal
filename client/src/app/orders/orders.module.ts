import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersComponent } from './orders.component';
import { OrderCreateComponent } from './order-create/order-create.component';
import { OrdersRoutingModule } from './orders-routing.module';
import { SharedModule } from '../shared/shared.module';
import { OrderEditComponent } from './order-edit/order-edit.component';
import { RemunerationComponent } from './remuneration/remuneration.component';
import { JdComponent } from './jd/jd.component';



@NgModule({
  declarations: [OrdersComponent, OrderCreateComponent, OrderEditComponent, RemunerationComponent, JdComponent],
  imports: [
    CommonModule,
    SharedModule,
    OrdersRoutingModule
  ],
  exports: [
  ]
})
export class OrdersModule { }
