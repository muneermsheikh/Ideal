import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmailComponent } from './email.component';
import { EmailCreateComponent } from './email-create/email-create.component';
import { SharedModule } from '../shared/shared.module';
import { EmailRoutingModule } from './email-routing.module';



@NgModule({
  declarations: [EmailComponent, EmailCreateComponent],
  imports: [
    CommonModule,
    SharedModule,
    EmailRoutingModule
  ]
})
export class EmailModule { }
