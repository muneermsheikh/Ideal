import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';


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
export class EmailRoutingModule { }
