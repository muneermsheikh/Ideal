import { Component, OnInit } from '@angular/core';
import { FormGroup, FormArray, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { Router, ActivatedRoute, Route } from '@angular/router';
import { timer, of } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ISource } from 'src/app/shared/models/candidateSource';
import { IClient } from 'src/app/shared/models/client';
import { ICandidate, ICandidateAddress, ICandidateCategory } from 'src/app/shared/models/ICand';
import { IProfession } from 'src/app/shared/models/profession';
import { UsersService } from '../../users.service';

@Component({
  selector: 'app-candidate-create',
  templateUrl: './candidate-create.component.html',
  styleUrls: ['./candidate-create.component.scss']
})

export class CandidateCreateComponent implements OnInit {
  createForm: FormGroup;
  candidate: ICandidate;
  pageTitle: string;
  candidateProfessions: IProfession[];
  candidateSources: ISource[];
  recruitmentAgents: IClient[];
  referredByDefaultId =  11;  // direct

  formErrors = {
   };

  validationMessages = {
      gender: {
        required: 'gender is required'
      },
      firstName: {
        required: 'first name is required',
        minLength: 'first name must be min 4 char long',
        maxLength: 'first Name must be max 25 char long'
      },
      email: {
        required: 'email is required',
        email : 'invalid email'
      },
      mobileNo: {
        required: 'mobile is required'
      }
    };


  constructor(private fb: FormBuilder,
              private accService: AccountService,
              private service: UsersService,
              private activatedRoute: ActivatedRoute,
              private route: Router)
  {
  }

  ngOnInit(): void {
    this.createForm = this.fb.group({
        id: [0],
        applicationNo: [0],
        applicationDated: ['2020-01-01T00:00:00'],
        // addedOn: [Date.toString()],
        gender: ['M', [Validators.required,
                Validators.maxLength(1),
                Validators.pattern('^(?:m|M|f|F)$')]],
        firstName: ['Sumit', [Validators.required, Validators.maxLength(25), Validators.minLength(4)]],
        secondName: ['Rajaram'],
        familyName: ['Thakkar'],
        knownAs: ['Sumit', [Validators.required, Validators.minLength(4), Validators.maxLength(15)]],
        aadharNo: ['123456789012', [Validators.minLength(12), Validators.maxLength(12)]],
        passportNo: ['L8349885', Validators.required],
        ecnr: ['ECR', Validators.required],
        mobileNo: ['1234567890', Validators.required],
        email: ['sumit@gmail.com', Validators.required],
        contactPreference: ['mobile'],
        dateOfBirth: ['1995-10-10T00:00:00', Validators.required],
        referredById: [null],
        sourceId: [null],
        address1: '',
        address2: '',
        city: ['', Validators.required],
        pin: '',
        state: '',
        country: ['', Validators.required],
        candidateCategories: this.fb.array([
          this.newCandidateCategory()
        ]),
      });

    this.getCandidateSources();
    this.getRecruitmentAgents();
    this.getCategories();

    const candidateId = +this.activatedRoute.snapshot.paramMap.get('id');

    if (candidateId) {
        // edit
        this.pageTitle = 'edit Candidate';
        this.getCandidate(candidateId);
      } else {
        // insert
        this.pageTitle = 'new candidate';
        this.blankOutTheFields();
        }

  }

    blankOutTheFields(): void {
        this.candidate = {
          id: 0,
          applicationNo: 0,
          applicationDated: Date.toString(),
          gender: 'M',
          firstName: '',
          secondName: '',
          familyName: '',
          knownAs: '',
          aadharNo: '',
          passportNo: '',
          ecnr: 'ECR',
          mobileNo: '',
          email: '',
          dateOfBirth: null,
          referredById: null,
          sourceId: null,
          address1: '',
          address2: '',
          city: '',
          pin: '',
          state: '',
          country: '',
          contactPreference: 'email',
          fullName: null,
          candidateCategories: []
      };
    }

    getCandidate(id: number): any {
      this.candidate = this.service.getCandidate(id).subscribe(
        (candidate: ICandidate) => {
          this.editCandidate(candidate);
          this.candidate = candidate;
        },
        (error: any) => console.log(error)
      );
    }

    editCandidate(candidate: ICandidate): any {
      this.createForm.patchValue(
        {
          id: candidate.id,
          applicationNo: candidate.applicationNo,
          applicationDated: candidate.applicationDated,
          gender: candidate.gender,
          firstName: candidate.firstName,
          secondName: candidate.secondName,
          familyName: candidate.familyName,
          knownAs: candidate.knownAs,
          aadharNo: candidate.aadharNo,
          passportNo: candidate.passportNo,
          ecnr: candidate.ecnr,
          mobileNo: candidate.mobileNo,
          email: candidate.email,
          contactPreference: candidate.contactPreference,
          dateOfBirth: candidate.dateOfBirth,
          referredById: candidate.referredById,
          sourceId: candidate.sourceId,
          address1: candidate.address1,
          address2: candidate.address2,
          city: candidate.city,
          pin: candidate.pin,
          state: candidate.state,
          country: candidate.country
        });

      if (candidate.candidateCategories !== null) {
        this.createForm.setControl('candidateCategories', this.setExistingCategories(candidate.candidateCategories));
        }

    }

 // categories
    setExistingCategories(cats: ICandidateCategory[]): FormArray {
      const formArray = new FormArray([]);
      cats.forEach( s => {
        formArray.push(this.fb.group( {
          categoryId: s.categoryId
          // name ignored, as using dropdown
        }));
      });
      return formArray;
    }

    candidateCategories(): FormArray {
      return this.createForm.get('candidateCategories') as FormArray;
    }

    newCandidateCategory(): FormGroup{
      return this.fb.group({
        categoryId: [null, Validators.required],
      });
    }

    pushNewCategory(): void {
      this.candidateCategories().push(this.newCandidateCategory());
    }

    removeCategory(i: number): any {
      const skillsFormArray = this.createForm.get('candidateCategories');
      skillsFormArray.markAsDirty();
      skillsFormArray.markAsTouched();
      this.candidateCategories().removeAt(i);
    }

    onSubmit(): any {
      const candidateVal = this.mapFormValuesToCandidateObject();
      if (candidateVal.id === null || candidateVal.id === 0)    // INSERT mode
      {
        this.service.addCandidate(candidateVal).subscribe(() => {
          this.route.navigate(['candidates']);
        }, error => {
          console.log(error);
        });
      } else {                          // EDIT mode
        this.service.updateCandidate(candidateVal).subscribe(() => {
          this.route.navigate(['candidates']);
      }, error => {
        console.log(error);
      });
      }

    }

    mapFormValuesToCandidateObject(): ICandidate{
        this.candidate.id = this.createForm.value.id ;
        this.candidate.applicationNo = this.createForm.value.applicationNo ?? 0;
        this.candidate.applicationDated = this.createForm.value.applicationDated;
        this.candidate.gender = this.createForm.value.gender;
        this.candidate.firstName = this.createForm.value.firstName;
        this.candidate.secondName = this.createForm.value.secondName;
        this.candidate.familyName = this.createForm.value.familyName;
        this.candidate.knownAs = this.createForm.value.knownAs;
        this.candidate.passportNo = this.createForm.value.passportNo;
        this.candidate.ecnr = this.createForm.value.ecnr;
        this.candidate.aadharNo = this.createForm.value.aadharNo;
        this.candidate.email = this.createForm.value.email;
        this.candidate.mobileNo = this.createForm.value.mobileNo;
        this.candidate.dateOfBirth = this.createForm.value.dateOfBirth;
        this.candidate.address1 = this.createForm.value.address1;
        this.candidate.address2 = this.createForm.value.address2;
        this.candidate.city = this.createForm.value.city;
        this.candidate.pin = this.createForm.value.pin;
        this.candidate.state = this.createForm.value.state;
        this.candidate.country = this.createForm.value.country;
        this.candidate.candidateCategories = this.createForm.value.candidateCategories;

        return this.candidate;
    }

    getCandidateSources(): void {
      this.service.getCandidateSources().subscribe(
        (sources: ISource[]) => {
          this.candidateSources = sources;
        },
        (error: any) => console.log(error)
      );
    }

    getRecruitmentAgents(): void {
      this.service.getRecruitmentAgencies().subscribe(
        (agents: IClient[]) => {
          this.recruitmentAgents = agents;
        },
        (error: any) => {
          console.log(error);
        }
      );
    }

    getCategories(): void {
      this.service.getProfessions().subscribe(
        (profs: IProfession[]) => {
          this.candidateProfessions = profs;
        },
        (error: any) => {
          console.log(error);
        }
      );
    }

    logValidationErrors(group: FormGroup = this.createForm): void {
      Object.keys(group.controls).forEach((key: string) => {
        const abstractControl = group.get(key);

        this.formErrors[key] = '';
        if (abstractControl && !abstractControl.valid &&
          (abstractControl.touched || abstractControl.dirty || abstractControl.value !== '')) {

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

    validateEmailNotTaken(): AsyncValidatorFn {
      return control => {
        return timer(500).pipe(
          switchMap(() => {
            if (!control.value)
            {
              return of(null);
            }
            return this.accService.checkEmailExists(control.value).pipe(
              map(res => {
                return res ? {emailExists: true} : null;
              })
            );
          })
        );
      };
    }

    validatePPNumberNotTaken(): AsyncValidatorFn {
      return control => {
        return timer(500).pipe(
          switchMap(() => {
            if (!control.value)
            {
              return of(null);
            }
            return this.service.checkPPNoExists(control.value).pipe(
              map(res => {
                return res ? {ppNoExists: true} : null;
              })
            );
          })
        );
      };
    }

    validateAadharNumberNotTaken(): AsyncValidatorFn {
      return control => {
        return timer(500).pipe(
          switchMap(() => {
            if (!control.value)
            {
              return of(null);
            }
            return this.service.checkAadharNoExists(control.value).pipe(
              map(res => {
                return res ? {aadharNoExists: true} : null;
              })
            );
          })
        );
      };
    }


}
