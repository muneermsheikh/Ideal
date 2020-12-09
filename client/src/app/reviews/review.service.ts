import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { IPaginationReview, PaginationReview } from '../shared/models/paginationReview';
import { IReview } from '../shared/models/review';
import { ReviewParams } from '../shared/models/reviewParams';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {
  private baseUrl = environment.apiUrl;
  reviews: IReview[] = [];
  pagination = new PaginationReview();
  params = new ReviewParams();
  private candidateSource = new BehaviorSubject<IReview>(null);


  constructor(private http: HttpClient) { }

  addReview(values: any): any {
    return this.http.post(this.baseUrl + 'dl/reviewitems', values).pipe(
      map((rvw: IReview) => {
        if (rvw) {
          console.log(rvw);
        }
      }, error => {
        console.log(error);
      })
    );
  }

  deleteReview(id: number): any {
    return this.http.delete(this.baseUrl + 'HR/candidate?id=' + id).pipe(
      map((response: number) => {
        if (response !== 0) {
          console.log('candidate deleted');
          this.candidateSource.next(null);
        }
        }, error => {
          console.log(error);
        }
      )
    );
  }


  updateReview(values: IReview): any {
    return this.http.put(this.baseUrl + 'dl/reviews', values).pipe(
      map((rvw: IReview) => {
        if (rvw) {
          console.log('Review updated'); }
        }, error => {
          console.log(error);
        }
      )
    );
  }

  getReview(id: number): any {
    const rvw = this.reviews.find(p => p.id === id);
    if (rvw) {
      return of(rvw);
    }
    return this.http.get<IReview>(this.baseUrl + 'dl/reviewenquiry/' + id);    // adds rvw items if dont exist
  }

  getReviews(useCache: boolean): any {

    // console.log('entered userService.getCandidates');

    if (useCache === false) {
      this.reviews = [];
    }

    if (this.reviews.length > 0 && useCache === true) {
      const pagesReceived = Math.ceil(this.reviews.length / this.params.pageSize);

      if (this.params.pageNumber <= pagesReceived) {
        this.pagination.data =
          this.reviews.slice((this.params.pageNumber - 1) * this.params.pageSize,
            this.params.pageNumber * this.params.pageSize);

        return of(this.pagination);
      }
    }

    let params = new HttpParams();

    if (this.params.reviewId !== 0) {
      params = params.append('id', this.params.reviewId.toString());
    }

    if (this.params.search)
    {
      params = params.append('search', this.params.search);
    }

    params = params.append('sort', this.params.sort);

    params = params.append('pageIndex', this.params.pageNumber.toString());
    params = params.append('pageSize', this.params.pageSize.toString());

    return this.http.get<IPaginationReview>(this.baseUrl + 'dl/reviewIndex', { observe: 'response', params })
      .pipe(
        map(response => {
          this.reviews = [...this.reviews, ...response.body.data];
          this.pagination = response.body;
          return this.pagination;
        }, error => {
          console.log(error);
        })
      );
  }

  getReviewStatus(): any {
    return this.http.get<IReview>(this.baseUrl + 'admin/reviewStatus');
  }

  getParams(): ReviewParams {
    return this.params;
  }

  setParams(params: ReviewParams): void {
    this.params = params;
  }

}
