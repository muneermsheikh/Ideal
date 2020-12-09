import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { IReview, IReviewItem } from 'src/app/shared/models/review';
import { IReviewStatus } from 'src/app/shared/models/reviewStatus';
import { ReviewService } from '../review.service';

@Component({
  selector: 'app-review-create',
  templateUrl: './review-create.component.html',
  styleUrls: ['./review-create.component.scss']
})
export class ReviewCreateComponent implements OnInit {

  createForm: FormGroup;
  review: IReview;
  pageTitle: string;
  reviewStatus: IReviewStatus[];
  formErrors = [];


   validationMessages = {
    visaAvailable: {
      required: 'Visa Available must be seleccted'
    },
 };


  constructor(private fb: FormBuilder,
              private service: ReviewService,
              private activatedRoute: ActivatedRoute,
              private route: Router)
  {
  }

  ngOnInit(): void {

    this.createForm = this.fb.group({
      id: 0,
      enquiryId: 0,
      enquiryNo: 0,
      enquiryDate: '',
      customerId: 0,
      customerName: '',
      reviewStatus: ['NotReviewed', Validators.required],
      reviewedBy: '',
      reviewedOn: ['', Validators.required],
      contractReviewItems: this.fb.array([
        this.newItem()
      ])
    });

    this.getReviewStatus();
    console.log('reviewstatus after returning');
    console.log(this.reviewStatus);

    const reviewId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (reviewId)
    {   // edit
      this.pageTitle = 'edit Contract Review';
      this.getReview(reviewId);
    }
    else {
      this.pageTitle = 'new Reviews';
      this.blankOutTheFields();
    }
  }

    blankOutTheFields(): void {
        this.review = {
          id: 0,
          enquiryId: 0,
          enquiryNo: 0,
          enquiryDate: '',
          customerId: 0,
          customerName: '',
          reviewStatus: '',
          reviewedBy: '',
          reviewedOn: '',
          contractReviewItems: []
      };
    }

    getReview(reviewId: number): any {
      this.review = this.service.getReview(reviewId).subscribe(
        (review: IReview) => {
          this.editReview(review);
          this.review = review;
          console.log('create component.ts - get Review');
          console.log(this.review);
      }, error => {
        console.log(error);
      });
    }

    editReview(review: IReview): any {
      this.createForm.patchValue(
        {
          id: review.id,
          enquiryId: review.enquiryId,
          enquiryNo: review.enquiryNo,
          enquiryDate: review.enquiryDate,
          customerId: review.customerId,
          customerName: review.customerName,
          reviewStatus: review.reviewStatus,
          reviewedBy: review.reviewedBy,
          reviewedOn: review.reviewedOn
        });

      if (review.contractReviewItems !== null && review.contractReviewItems !== undefined) {
        this.createForm.setControl('contractReviewItems', this.setExistingItems(review.contractReviewItems));
        }
    }

 // contractReviewItems
    setExistingItems(items: IReviewItem[]): FormArray {
      const formArray = new FormArray([]);
      items.forEach( s => {
        formArray.push(this.fb.group( {
          id: s.id,
          enquiryId: s.enquiryId,
          enquiryItemId: s.enquiryItemId,
          categoryItemId: s.categoryItemId,
          categoryName: s.categoryName,
          quantity: s.quantity,
          technicallyFeasible: s.technicallyFeasible,
          commerciallyFeasible: s.commerciallyFeasible,
          logisticallyFeasible: s.logisticallyFeasible,
          visaAvailable: s.visaAvailable,
          documentationWillBeAvailable: s.documentationWillBeAvailable,
          historicalStatusAvailable: s.historicalStatusAvailable,
          salaryOfferedFeasible: s.salaryOfferedFeasible,
          serviceChargesInINR: s.serviceChargesInINR,
          feeFromClientCurrency: s.feeFromClientCurrency,
          feeFromClient: s.feeFromClient,
          status: s.status,
          reviewedOn: s.reviewedOn,
          reviewedBy: s.reviewedBy
        }));
      });
      return formArray;
    }

    contractReviewItems(): FormArray {
      return this.createForm.get('contractReviewItems') as FormArray;
    }

    newItem(): FormGroup{
      return this.fb.group({
        id: [null],         // not editable
        enquiryId: 0,       // not editable
        enquiryItemId: 0,   // not editable
        enquiryNo: 0,       // not editable
        categoryItemId: 0,  // not editable
        categoryName: '',   // not editable
        quantity: 0,        // not editable
        technicallyFeasible: [false, Validators.required],
        commerciallyFeasible: [false, Validators.required],
        logisticallyFeasible: [false, Validators.required],
        visaAvailable: [false, Validators.required],
        documentationWillBeAvailable: [false, Validators.required],
        historicalStatusAvailable: [false, Validators.required],
        salaryOfferedFeasible: [false, Validators.required],
        serviceChargesInINR: '',
        feeFromClientCurrency: '',
        feeFromClient: null,
        status: ['', Validators.required],
        reviewedOn: '',
        reviewedBy: ''
      });
    }

    pushNewItem(): void {
      this.contractReviewItems().push(this.newItem());
    }

    removeItem(i: number): any {
      const formArray = this.createForm.get('contractReviewItems');
      formArray.markAsDirty();
      formArray.markAsTouched();
      this.contractReviewItems().removeAt(i);
    }

    onSubmit(): any {
      const reviewVal = this.mapFormValuesToReviewObject();
      if (reviewVal.id === null || reviewVal.id === 0)    // INSERT mode
      {
        this.service.addReview(reviewVal).subscribe(() => {
          this.route.navigate(['review']);
        }, error => {
          console.log(error);
        });
      } else {                          // EDIT mode
        this.service.updateReview(reviewVal).subscribe(() => {
          this.route.navigate(['review']);
      }, error => {
        console.log(error);
      });
      }
    }

    getReviewStatus(): any {
      return this.service.getReviewStatus().subscribe(
        (status: IReviewStatus[]) => {
          this.reviewStatus = status;
      }, error => {
        console.log(error);
      });
    }


    mapFormValuesToReviewObject(): IReview{
        this.review.id = this.createForm.value.id ;
        this.review.enquiryNo = this.createForm.value.enquiryNo ?? 0;
        this.review.enquiryDate = this.createForm.value.enquiryDate;
        this.review.customerName = this.createForm.value.customerName;
        this.review.reviewStatus = this.createForm.value.reviewStatus;
        this.review.reviewedBy = this.createForm.value.reviewedBy;
        this.review.reviewedOn = this.createForm.value.reviewedOn;
        this.review.contractReviewItems = this.createForm.value.contractReviewItems;
        return this.review;
    }
}
