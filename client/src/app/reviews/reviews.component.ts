import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { IReview } from '../shared/models/review';
import { ReviewParams } from '../shared/models/reviewParams';
import { ReviewService } from './review.service';

@Component({
  selector: 'app-reviews',
  templateUrl: './reviews.component.html',
  styleUrls: ['./reviews.component.scss']
})
export class ReviewsComponent implements OnInit {

  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  reviews: IReview[];
  form: FormGroup;
  errors: string[];

  params = new ReviewParams();
  totalCount: number;
  sortSelected = 'asc';
  sortOptions = [
    { name: 'Ascending by category', value: 'categoryNameasc' },
    { name: 'Descending by category', value: 'categoryNameDesc' },
    { name: 'Ascending by City Name', value: 'cityNameTypeAsc' },
    { name: 'Descending by City Name', value: 'cityNameDesc' }
  ];

  constructor(private service: ReviewService, private fb: FormBuilder,
              private router: Router) { }

  ngOnInit(): void {
    this.getReviews();
  }

  editButtonClick(reviewId: number): void {
    this.router.navigate(['/reviewEdit', reviewId]);
  }

  getReviews(useCache = false): void {
      this.service.getReviews(useCache).subscribe(response => {
      this.reviews = response.data;
      this.totalCount = response.count;
      this.params.pageNumber = response.pageIndex;
      this.params.pageSize = response.pageSize;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any): void {
    const params = this.service.getParams();
    if (params.pageNumber !== event) {
      params.pageNumber = event;
      this.service.setParams(params);
      this.getReviews(true);
    }
  }

  onSearch(): void{
    const params = this.service.getParams();
    params.search = this.searchTerm.nativeElement.value;
    params.pageNumber = 1;
    this.service.setParams(params);

    this.getReviews();
  }


  onReset(): any {
    this.searchTerm.nativeElement.value = '';
    this.params = new ReviewParams();
    this.getReviews();
  }

  onSortSelected(sort: string): any {
    const params = this.service.getParams();
    params.sort = sort;
    this.service.setParams(params);
    this.getReviews();
  }

}
