import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { LotService } from 'src/app/services/lot.service';
import { Lot } from 'src/app/models/lot.model';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category.model';

@Component({
  selector: 'app-add-lot',
  templateUrl: './add-lot.component.html',
  styleUrls: ['./add-lot.component.css']
})
export class AddLotComponent implements OnInit {

  constructor(private lotService: LotService,
    private router: Router,
    private alertService: AlertService,
    private formBuilder: FormBuilder,
    private categoryService: CategoryService) { }

  addLotForm: FormGroup;
  loading = false;
  submitted = false;
  categories: Category[] = [];
  image: any;

  get f() { return this.addLotForm.controls; }

  onSubmit() {
    this.submitted = true;
    if (this.addLotForm.invalid) {
      return;
    }
    this.loading = true;
    if (this.image)
      this.image = this.image.split("base64,")[1];
    this.lotService.create(<Lot>{
      name: this.f.name.value,
      initialPrice: this.f.initialPrice.value,
      beginDate: this.f.date.value[0],
      endDate: this.f.date.value[1],
      description: this.f.description.value,
      category: <Category>{ categoryId: this.f.category.value },
      image: this.image
    })
      .subscribe(
        () => {
          this.alertService.success('Lot created');
          this.router.navigate(['']);
        },
        error => {
          this.alertService.error(error.error.message);
          this.loading = false;
        });
  }

  imageUpload(e) {
    var file = e.dataTransfer ? e.dataTransfer.files[0] : e.target.files[0];
    var pattern = /image-*/;
    var reader = new FileReader();
    if (!file.type.match(pattern)) {
      this.alertService.error("Invalid image format.")
      return;
    }
    reader.onload = ((e) => {
      let reader = e.target;
      this.image = reader.result;
    }).bind(this);
    reader.readAsDataURL(file);
  }

  ngOnInit() {
    this.categoryService.getAll().subscribe(
      categories => this.categories = categories
    );
    this.addLotForm = this.formBuilder.group({
      name: ['', [Validators.required, Validators.maxLength(50)]],
      initialPrice: ['', [Validators.required, Validators.min(0.01)]],
      date: ['', Validators.required],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      category: ['', Validators.required]
    });
  }

}
