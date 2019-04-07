import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import {
  MatButtonModule,
  MatCardModule,
  MatDialogModule,
  MatInputModule,
  MatTableModule,
  MatToolbarModule,
  MatMenuModule,
  MatIconModule,
  MatProgressSpinnerModule,
  MatGridListModule,
  MatDividerModule,
  MatExpansionModule,
  MatDatepickerModule,
  MatOptionModule,
  MatSelectModule,
  MatNativeDateModule
} from '@angular/material';

import { DragDropModule } from '@angular/cdk/drag-drop';

@NgModule({
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatDividerModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatOptionModule,
    MatSelectModule,
    MatNativeDateModule,
    DragDropModule
  ],
  exports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatInputModule,
    MatDialogModule,
    MatTableModule,
    MatMenuModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatDividerModule,
    MatExpansionModule,
    MatDatepickerModule,
    MatOptionModule,
    MatSelectModule,
    MatNativeDateModule,
    DragDropModule
  ]
})
export class MaterialExportsModule {}
