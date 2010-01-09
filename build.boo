configuration = "release"
test_assembly = "src/FluentLinqToSql.Tests/bin/${configuration}/FluentLinqToSql.Tests.dll"
dbtest_assembly = "src/FluentLinqToSql.DatabaseTests/bin/${configuration}/FluentLinqToSql.DatabaseTests.dll"
artest_assembly = "src/FluentLinqToSql.Tests.ActiveRecord/bin/${configuration}/FluentLinqToSql.Tests.ActiveRecord.dll"
dbtest_enabled = true

target default, (init, compile, test, createDb, dbTest, deploy, package):
  pass

target ci, (init, compile, createDb, coverage, deploy, package):
  pass

target init:
  rmdir("build")

target noDatabase:
  dbtest_enabled = false  

target compile:
  msbuild(file: "FluentLinqToSql.sln", configuration: "release")

target test:
  nunit(toolPath: "lib/nunit/nunit-console-x86.exe", assemblies: (test_assembly, artest_assembly))

target createDb:
  if not dbtest_enabled:
    print "Skipping database creation"
    return
  print "Trying to create test db"
  #nunit(assembly: dbtest_assembly, include: "CreateDatabase")
  exec("lib/Tarantino/DatabaseDeployer.exe", "Rebuild (local) FluentLinqToSql db")
    
  print "Test database created"

target dbTest:
  if not dbtest_enabled:
    print "Skipping database tests"
    return
  nunit(assembly: dbtest_assembly)
  
target deploy:
  with FileList():
    .Include("src/FluentLinqToSql/bin/${configuration}/*.{dll,pdb,xml}")
    .Include("readme.txt")
    .Include("License.txt")
    .Flatten(true)
    .ForEach def(file):
      file.CopyToDirectory("build/${configuration}")
      
target package:
  zip("build/${configuration}", "build/FluentLinqToSql.zip")

target coverage:
  ncover_path = "c:/program files (x86)/ncover"
  app_assemblies = ("FluentLinqToSql",)
  teamcity_launcher = env("teamcity.dotnet.nunitlauncher")
  
  #ensure the other test assemblies are in the right place...
  System.IO.File.Copy(dbtest_assembly, "src/FluentLinqToSql.Tests/bin/${configuration}/FluentLinqToSql.DatabaseTests.dll", true)
  System.IO.File.Copy(artest_assembly, "src/FluentLinqToSql.Tests/bin/${configuration}/FluentLinqToSql.Tests.ActiveRecord.dll", true)


  with ncover():
    .toolPath = "${ncover_path}/NCover.console.exe"
    .reportDirectory = "build/Coverage"
    .workingDirectory = "src/FluentLinqToSql.Tests/bin/${configuration}"
    .applicationAssemblies = app_assemblies
    .program = "${teamcity_launcher} v2.0 x86 NUnit-2.4.6"
    .testAssembly = "FluentLinqToSql.Tests.dll;FluentLinqToSql.DatabaseTests.dll;FluentLinqToSql.Tests.ActiveRecord.dll"
    .excludeAttributes = "System.Runtime.CompilerServices.CompilerGeneratedAttribute"

  with ncover_explorer():
    .toolPath = "${ncover_path}/NCoverExplorer.console.exe"
    .project = "FluentLinqToSql"
    .reportDirectory = "build/coverage"
