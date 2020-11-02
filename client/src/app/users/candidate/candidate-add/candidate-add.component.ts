import { Component, OnInit } from '@angular/core';
import { AbstractControl, AsyncValidatorFn, Form, FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { of, timer } from 'rxjs';
import { map, switchMap } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ProfessionService } from 'src/app/profession/profession.service';
import { ISource } from 'src/app/shared/models/candidateSource';
import { IClient } from 'src/app/shared/models/client';
import { ICandidate } from 'src/app/shared/models/ICand';
import { IIndustryType } from 'src/app/shared/models/industryType';
import { IProfession } from 'src/app/shared/models/profession';
import { ISkillLevel } from 'src/app/shared/models/skillLevel';
import { UsersService } from '../../users.service';

@Component({
  selector: 'app-candidate-add',
  templateUrl: './candidate-add.component.html',
  styleUrls: ['./candidate-add.component.scss']
})

  export class CandidateAddComponent implements OnInit {
      candidateToEdit: ICandidate;
      professions: IProfession[] = [];
      industryTypes: IIndustryType[] = [];
      skillLevels: ISkillLevel[] = [];
      form: FormGroup;
      formArrayAdd: FormArray;
      formArrayCat: FormArray;
      recruitmentAgencies: IClient[] = [];
      candidateSources: ISource[] = [];

      // formErrors: string[];

      validationMessages = {
        firstName: {required: 'first name required'},
        familyName: {required: 'family name required'},
        email: {required: 'email required'},
        confirmEmail: {
          required: 'confirm email is required',
          matchEmail: 'email and confirm Email do not match'
        },
        pPNo: {required: 'Passport Number is required'},
        mobileNo: {required: 'Mobile No. is required'},
        city: {required: 'City name is required'},
        professionId: {required: 'Profession should be selected'}
      };

      formErrors = {
        firstName: '',
        familyName: '',
        email: '',
        confirmEmail: '',
        pPNo: '',
        referredById: '',
        sourceId: '',
        mobileNo: '',
        city: '',
        professionId: ''
      };

      constructor(private service: UsersService,
                  private profService: ProfessionService,
                  private acctService: AccountService,
                  private fb: FormBuilder, private router: Router,
                  private activatedRoute: ActivatedRoute)
      {
      }

      ngOnInit(): void {
        this.getProfessions();
        this.getCandidateSources();
        this.getRecruitmentAgencies();
        this.createForm();

        this.activatedRoute.paramMap.subscribe(params => {
          const candId = +params.get('id');
          if (candId) {

          }
        });
      }

      getCandidate(id: number): void {
        this.service.getCandidate(id).subscribe( (cand: ICandidate) => {
          this.editCandidate(cand);
        }, error => {
          console.log(error);
        });
      }

      editCandidate(candidt: ICandidate): void {
        this.form.patchValue( {
          id: candidt.id,
          applicationNo: candidt.applicationNo,
          applicationDated: candidt.applicationDated,
          addedOn: candidt.addedOn,
          gender: candidt.gender,
          firstName: candidt.firstName,
          secondName: candidt.secondName,
          familyName: candidt.familyName,
          knownAs: candidt.knownAs,
          dOB: candidt.dOB,
          pPNo: candidt.ppNo,
          referredById: candidt.referredById,
          sourceId: candidt.sourceId,
          eCNR: candidt.eCNR,
          aadharNo: candidt.aadharNo,
          mobileNo: candidt.mobileNo,
          contactPreference: candidt.contactPreference,
          emailGroup: {
            email: candidt.email,
            confirmEmail: candidt.email
          }
        });
      }

      getProfessions(): any {
        this.service.getProfessions().subscribe(response =>
          {this.professions = response; },
          error => {console.log(error); } );
      }

      getIndustryTypes(): any {
        this.profService.getIndustryTypes().subscribe(response =>
          {this.industryTypes = response; },
          error => {console.log(error); } );
      }

      getSkillLevels(): any {
        this.profService.getSkillLevels().subscribe(response =>
          {this.skillLevels = response; },
          error => {console.log(error); } );
      }

      getRecruitmentAgencies(): any {
        this.service.getRecruitmentAgencies().subscribe(response => {
          this.recruitmentAgencies = response;
        }, error => {
          console.log(error);
        });
      }

      getCandidateSources(): any {
        this.service.getCandidateSources().subscribe(response => {
          this.candidateSources = response;
        }, error => {
          console.log(error);
        });
      }

      createForm(): any {
        this.form = this.fb.group({
          applicationNo: [0],
          applicationDated:  ['2020-10-10T00:00:00', [Validators.required]],
          gender:  ['M', [Validators.required, Validators.maxLength(1),
            Validators.pattern('^(?:m|M|f|F)$')]],
          firstName:  ['Candidate 1022', [Validators.required, Validators.maxLength(25), Validators.minLength(5)]],
          secondName:  ['secondname 1022'],
          familyName:  ['family1022'],
          knownAs: ['known1022', [Validators.required, Validators.maxLength(10), Validators.minLength(4)]],
          dOB: ['1985-10-10T00:00:00'],
          eCNR: [false],
          ppNo: ['X3985855', [Validators.required, Validators.minLength(5), Validators.maxLength(15)
          ]],
          referredById: [null],
          sourceId: [null],
          aadharNo: [null, [Validators.minLength(12), Validators.maxLength(12),
            [this.validateAadharNumberNotTaken]  // asynchronos validation
          ]] ,
          mobileNo: ['9834949332', [Validators.required, Validators.minLength(10)]],
          emailGroup: this.fb.group( {
            email: ['1022@gmail.com', [
              Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')
            ]],
            confirmEmail: ['1022@gmail.com', [Validators.required,
              Validators.pattern('^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$')]]
          }),
          formArrayCat: this.fb.array([this.createProfessionForm()]),
          formArrayAdd: this.fb.array([this.createAddressForm()])
          });
      }

      createAddressForm(): FormGroup {
        return this.fb.group({
          addressType: ['R', [Validators.required, Validators.maxLength(1),
            Validators.pattern('^(?:r|R|o|O|H|h|N|n)$')]],
          address1: ['25/50 BDD Blocks'],
          address2: ['Worli'],
          city: ['Mumbai', [Validators.required, Validators.minLength(4)]],
          pin: ['400018'],
          state: ['Maharashtra'],
          district: ['Mumbai'],
          country: ['India', [Validators.required]]
          });
      }

      get AddressControls(): FormArray {
        return this.form.get('formArrayAdd') as FormArray;  // ['controls'];
      }


      addAddress(): void {
        this.formArrayAdd = this.form.get('formArrayAdd') as FormArray;
        this.formArrayAdd.push(this.createAddressForm());
      }

      removeAddress(i: number): void {
        this.formArrayAdd.removeAt(i);
      }

      logAddressValue(): void {
        console.log(this.formArrayAdd.value);
      }

      createProfessionForm(): FormGroup {
        return this.fb.group({
          professionId: [10, [Validators.required]]
        });
      }

      get CategoryControls(): FormArray {
        // return this.form.get('categoryFormArray')['controls'];
        return this.form.get('formArrayCat') as FormArray; // ['controls'];
      }

      addProfession(): void {
        this.formArrayCat = this.form.get('formArrayCat') as FormArray;
        this.formArrayCat.push(this.createProfessionForm());
      }

      removeProfession(i: number): void {
        this.formArrayCat.removeAt(i);
      }

      onSubmit(): any {

        this.service.addCandidate(this.form.value).subscribe(response => {

        this.router.navigateByUrl('candidate');
        }, error => {
          console.log(error);
          this.formErrors = error.formErrors;
        });
      }

      matchEmail(group: AbstractControl): {[key: string]: any} | null
      {
        const emailControl = group.get('email');
        const confirmEmailControl = group.get('confirmEmail');

        if (emailControl.value === confirmEmailControl.value)
        {
          return null;
        } else {
            return {emailMismatch: true};
        }
      }

      validateEmailNotTaken(): AsyncValidatorFn {
        return control => {
          return timer(500).pipe(
            switchMap(() => {
              if (!control.value)
              {
                return of(null);
              }
              return this.acctService.checkEmailExists(control.value).pipe(
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


      logValidationErrors(group: FormGroup = this.form): void
      {

        Object.keys(group.controls).forEach((key: string) => {
          const abstractControl = group.get(key);

          this.formErrors[key] = '';
          if (abstractControl && abstractControl.valid &&
            (abstractControl.touched && abstractControl.dirty))
          {
            const messages = this.validationMessages[key];

            for (const errorKey in abstractControl.errors)
            {
              if (errorKey) {
                this.formErrors[key] += messages[errorKey] + ' ';
              }
            }
          }

          if (abstractControl instanceof FormGroup)
          {
            this.logValidationErrors(abstractControl);
          }

          if (abstractControl instanceof FormArray)
          {
            for (const control of abstractControl.controls)
            {
              if (control instanceof FormGroup)
              {
                this.logValidationErrors(control);
              }
            }
          }
      });
    }
}
