import { Component, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ProfessionService } from 'src/app/profession/profession.service';
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
      professions: IProfession[] = [];
      industryTypes: IIndustryType[] = [];
      skillLevels: ISkillLevel[] = [];
      form: FormGroup;
      addressFormArray: FormArray;
      categoryFormArray: FormArray;
      errors: string[];

      constructor(private service: UsersService,
                  private profService: ProfessionService,
                  private fb: FormBuilder, private router: Router) { }

      ngOnInit(): void {
        this.getProfessions();
        this.getIndustryTypes();
        this.getSkillLevels();
        this.createForm();
        this.getProfessions();
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

      createForm() {
        this.form = this.fb.group({
        applicationDated:  [null, [Validators.required]],
        gender:  ['M', [Validators.required]],
        firstName:  [null, [Validators.required]],
        secondName:  [null],
        familyName:  [null],
        knownAs: [null, [Validators.required]],
        dOB: [null],
        eCNR: [false],
        categoryFormArray: this.fb.array([ this.createProfessionForm() ]),
        ppNo:  [null],
        aadharNo:  [null] ,
        mobileNo:  [null, [Validators.required]],
        email:  [null, [Validators.email]],
        addressFormArray: this.fb.array([this.createAddressForm() ])
      });
    }

   createAddressForm(): FormGroup {
    return this.fb.group({
      addressType: [null, [Validators.required]],
      address1: [null],
      city: [null, [Validators.required]],
      pin: [null],
      state: [null],
      district: [null],
      country: [null]
      });
    }

    get AddressControls() {
      return this.form.get('addressFormArray')['controls'];
    }

    addAddress(): void {
      this.addressFormArray = this.form.get('addressFormArray') as FormArray;
      this.addressFormArray.push(this.createAddressForm());
    }

    removeAddress(i: number) {
      this.addressFormArray.removeAt(i);
    }

    logAddressValue() {
      console.log(this.addressFormArray.value);
    }

    createProfessionForm(): FormGroup {
      return this.fb.group({
        professionId: [null, [Validators.required]],
        name: [null, [Validators.required]],
        industryTypeId: [null],
        skillLevelId: [null]
      });
   }

  get CategoryControls() {
    return this.form.get('categoryFormArray')['controls'];
  }

  addProfession(): void {
      this.categoryFormArray = this.form.get('categoryFormArray') as FormArray;
      this.categoryFormArray.push(this.createProfessionForm());
    }

    removeProfession(i: number) {
      this.categoryFormArray.removeAt(i);
    }


    onSubmit() {
        console.log(this.form.value);
        this.service.addCandidate(this.form.value).subscribe(response => {

        this.router.navigateByUrl('candidate');
      }, error => {
        console.log(error);
        this.errors = error.errors;
      });
    }

  }
