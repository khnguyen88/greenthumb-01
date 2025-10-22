import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardBase } from './dashboard-base';

describe('DashboardBase', () => {
  let component: DashboardBase;
  let fixture: ComponentFixture<DashboardBase>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DashboardBase]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DashboardBase);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
