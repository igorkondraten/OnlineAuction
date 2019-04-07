import { MaterialExportsModule } from './material-exports.module';

describe('MaterialExportsModule', () => {
  let materialExportsModule: MaterialExportsModule;

  beforeEach(() => {
    materialExportsModule = new MaterialExportsModule();
  });

  it('should create an instance', () => {
    expect(materialExportsModule).toBeTruthy();
  });
});
