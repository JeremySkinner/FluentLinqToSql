configuration = "release"
test_assembly = "src/FluentLinqToSql.Tests/bin/${configuration}/FluentLinqToSql.Tests.dll"
dbtest_assembly = "src/FluentLinqToSql.DatabaseTests/bin/${configuration}/FluentLinqToSql.DatabaseTests.dll"
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
  nunit(assembly: test_assembly)

target createDb:
  if not dbtest_enabled:
    print "Skipping database creation"
    return
  print "Trying to create test db"
  nunit(assembly: dbtest_assembly, include: "CreateDatabase")
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
  
  #ensure the dbtest assembly is in the right place...
  cp(dbtest_assembly, "src/FluentLinqToSql.Tests/bin/${configuration}/FluentLinqToSql.DatabaseTests.dll")

  with ncover():
    .toolPath = "${ncover_path}/NCover.console.exe"
    .reportDirectory = "build/Coverage"
    .workingDirectory = "src/FluentLinqToSql.Tests/bin/${configuration}"
    .applicationAssemblies = app_assemblies
    .program = "${teamcity_launcher} v2.0 x86 NUnit-2.4.6"
    .testAssembly = "FluentLinqToSql.Tests.dll;FluentLinqToSql.DatabaseTests.dll"
    .excludeAttributes = "System.Runtime.CompilerServices.CompilerGeneratedAttribute"

  with ncover_explorer():
    .toolPath = "${ncover_path}/NCoverExplorer.console.exe"
    .project = "FluentLinqToSql"
    .reportDirectory = "build/coverage"
