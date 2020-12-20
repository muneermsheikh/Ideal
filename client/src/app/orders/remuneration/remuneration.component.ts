import { Component, OnInit } from '@angular/core';
import { AsyncValidatorFn, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { of, timer } from 'rxjs';
import { switchMap } from 'rxjs/operators';
import { IRemunDto } from 'src/app/shared/models/enquiry';
import { OrdersService } from '../orders.service';

@Component({
  selector: 'app-remuneration',
  templateUrl: './remuneration.component.html',
  styleUrls: ['./remuneration.component.scss']
})

export class RemunerationComponent implements OnInit {
  // private user: IUser;
  form: FormGroup;
  enqRemunerations: IRemunDto;
  pageTitle: string;
  
  formErrors = {
  };

  constructor(private fb: FormBuilder,
              private service: OrdersService,
              private activatedRoute: ActivatedRoute,
              private route: Router,
              private toastr: ToastrService)
  {
  }

  ngOnInit(): void {
  
    this.form = this.fb.group({
      enquiryId: [0],
      enquiryNo: [null, Validators.required],
      enquiryDate:  [null, Validators.required],
      customerName:  [null, Validators.required],
      remunerationItems: this.fb.array([
          this.newRemunerations()
        ]),
      });

    const enquiryId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (enquiryId) {
        this.pageTitle = 'edit Remunerations';
        this.enqRemunerations = this.getRemunerations(enquiryId);
    }
  }

  newRemunerations(): FormGroup{
    return this.fb.group(
    {
      enquiryId: [null, Validators.required],
      enquiryItemId: [null, Validators.required],
      categoryName: [null],
      contractPeriodInMonths: [null, Validators.required],
      salaryCurrency: [null, Validators.required],
      salaryMin: [null, Validators.required],
      salaryMax: [null],
      salaryNegotiable: [null],
      housing: [null],
      housingAllowance: [null],
      food: [null],
      foodAllowance: [null],
      transport: [null],
      transportAllowance: [null],
      otherAllowance: [null],
      leaveAvailableAfterHowmanyMonths: [null, Validators.required],
      leaveEntitlementPerYear: [null, Validators.required],
    });
  }

  getRemunerations(id: number): any {
    this.enqRemunerations = this.service.getRemunerations(id).subscribe(
      (remun: IRemunDto) => {
        this.enqRemunerations = remun;
        this.editRemunerations(remun);
        console.log('getRemunerations', remun);
      },
      (error: any) => console.log(error)
    );
  }

  editRemunerations(remun: IRemunDto): any {
      this.form.patchValue(
      {
        enquiryId: remun.enquiryId,
        enquiryNo: remun.enquiryNo,
        enquiryDate: remun.enquiryDate,
        customerName: remun.customerName
      });

      this.form.setControl('remunerations', this.setExistingRemunerations(remun.remunerations));
      
  }

  setExistingRemunerations(rems: any): FormArray {
      const formArray = new FormArray([]);
      rems.forEach( s => {
          formArray.push(this.fb.group( {
          enquiryId: s.enquiryId,
          enquiryItemId: s.enquiryItemId,
          categoryName: s.categoryName,
          contractPeriodInMonths: s.contractPeriodInMonths,
          salaryCurrency: s.salaryCurrency,
          salaryMin: s.salaryMin,
          salaryMax: s.salaryMax,
          salaryNegotiable: s.salaryNegotiable,
          housing: s.housing,
          housingAllowance: s.housingAllowance,
          food: s.food,
          foodAllowance: s.foodAllowance,
          transport: s.transport,
          transportAllowance: s.transportAllowance,
          otherAllowance: s.otherAllowance,
          leaveAvailableAfterHowmanyMonths: s.leaveAvailableAfterHowmanyMonths,
          leaveEntitlementPerYear: s.leaveEntitlementPerYear,
          updatedOn: s.updatedOn
          }));
        });
      return formArray;
  }

  remunerationItems(): FormArray {
    return this.form.get('remunerationItems') as FormArray;
  }


  mapFormValuesToRemunObject(): IRemunDto{
    this.enqRemunerations.enquiryId = this.form.value.enquiryId ;
    this.enqRemunerations.enquiryNo = this.form.value.enquiryNo;
    this.enqRemunerations.customerName = this.form.value.customerName;
    this.enqRemunerations.enquiryDate = this.form.value.enquiryDate;
    this.enqRemunerations.remunerations = this.form.value.enquiryItems;
    return this.enqRemunerations;
  }

  logValidationErrors(group: FormGroup = this.form): void {
    Object.keys(group.controls).forEach((key: string) => {
      const abstractControl = group.get(key);

      this.formErrors[key] = '';
      if (abstractControl && !abstractControl.valid &&
        (abstractControl.touched || abstractControl.dirty || abstractControl.value !== ''))
      {
          for (const errorKey in abstractControl.errors) {
            if (errorKey) {
              // this.formErrors[key] += messages[errorKey] + '';
            }
          }
        }

      if (abstractControl instanceof FormGroup) {
        this.logValidationErrors(abstractControl);
      }
    });
  }

  valuesValidate(vals: any) {
    var rems = this.enqRemunerations.remunerations;
    var errValidate='';
    rems.forEach(s => {
      errValidate += !s.food && s.foodAllowance===0 ? 'either free food or food allowance must be selected' : '';
      errValidate += !s.housing && s.housingAllowance ===0 ? 'either free housing or housing allowance must be selected' : '';
      errValidate += !s.transport && s.transportAllowance ===0 ? 'either free transport or transport allowance must be selected' : '';
    })
    if (errValidate !=='') {
      this.toastr.error(errValidate);
      return false;
    } else {
      return true;
    }
  }
  onSubmit(): any {
    if (!this.valuesValidate) { 
      return;
    }
    const remVal = this.mapFormValuesToRemunObject();
    this.service.updateRemunerations(remVal).subscribe(() => {
    this.route.navigate(['enquiry/' + remVal.enquiryId]);
    }, error => {
      console.log(error);
    });
  }

  
}

