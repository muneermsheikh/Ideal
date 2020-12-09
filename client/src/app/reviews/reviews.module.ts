import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReviewsComponent } from './reviews.component';
import { ReviewCreateComponent } from './review-create/review-create.component';
import { ReviewRoutingModule } from './review-routing.module';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [ReviewsComponent, ReviewCreateComponent],
  imports: [
    CommonModule,
    SharedModule,
    ReviewRoutingModule
  ],
  exports: [
    ReviewsComponent,
    ReviewCreateComponent,
    SharedModule
  ]
})
export class ReviewsModule { }
