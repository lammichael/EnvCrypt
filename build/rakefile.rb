require 'albacore'
require 'nokogiri'

################################
#  Variables
################################
#  Common directory locations
CheckoutDir=File.expand_path('.', File.join(File.dirname(__FILE__), '..'))
SourceDir=File.join(CheckoutDir, 'src')
OutputDir=File.join(CheckoutDir, 'out')
NuGetPackagesDir=File.join(SourceDir, 'packages')

CommonAssemblyInfoFilePath=File.join SourceDir, 'CommonAssemblyInfo.cs'

#  What we're building
BuildConfiguration='Release'
SLNFilePath=File.join SourceDir, 'EnvCrypt.Core.sln'

#  Nuget
NuGetExe = File.join CheckoutDir, 'tools', 'NuGet.2.8.5', 'NuGet.exe'
NuSpecFilePath = File.join(SourceDir, 'EnvCrypt.Core.nuspec')

# Test runner
NUnitExe = File.join(NuGetPackagesDir, 'NUnit.Runners.2.6.4', 'tools', 'nunit-console.exe')
OpenCoverExe = File.join(NuGetPackagesDir, 'OpenCover.4.5.3723', 'OpenCover.Console.exe')
ReportGeneratorExe = File.join(NuGetPackagesDir, 'ReportGenerator.2.1.7.0', 'tools', 'ReportGenerator.exe')
OpenCoverReportFilePath = File.join(OutputDir, 'UnitTestCoverageResults.xml')


################################
#  Setup
################################
desc 'restore all nugets as per the packages.config files'
task(:packagerestore) do
  sh(NuGetExe, 'restore', SLNFilePath)
end


task :createoutputdir do
  Dir.mkdir OutputDir unless File.directory?(OutputDir)
end


task :cleanoutput do
  rm_rf OutputDir
end


################################
#  VS
################################
desc 'MSBuild clean'
build :msbuildclean do |b|
  b.sln  = SLNFilePath
  b.target = 'Clean'
  b.prop 'Configuration', BuildConfiguration	# call with 'key, value', to specify a MsBuild property
  b.logging = 'normal'					# detailed logging mode. The available verbosity levels are: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]
  # b.be_quiet						# opposite of the above
  b.nologo							# no Microsoft/XBuild header output
  #b.prop 'NoWarn', '1591'			# Don't warn about missing XML doc
end


desc 'MSBuild compile'
build :msbuildcompile do |b|
  b.sln  = SLNFilePath
  b.target = 'Build'				# call with an array of targets or just a single target
  b.prop 'Configuration', BuildConfiguration	# call with 'key, value', to specify a MsBuild property
  #b.clp 'ShowEventId'				# any parameters you want to pass to the console logger of MsBuild
  b.logging = 'm'					# detailed logging mode. The available verbosity levels are: q[uiet], m[inimal], n[ormal], d[etailed], and diag[nostic]
  # b.be_quiet						# opposite of the above
  b.prop 'NoWarn', '1591'			# Don't warn about missing XML doc
end



################################
#  Tests
################################
task :nunitunittest do
  sh(NUnitExe, get_nunit_console_args)
end


task :opencovernunitunittest do
  sh(OpenCoverExe,
    '-target:' + NUnitExe,
    '"-targetargs:' + get_nunit_console_args().join(' ') + '"',
    '-output:' + OpenCoverReportFilePath,
	'-register:Path32',
	'"-filter:+[*]* -[FluentAssertions.Core]FluentAssertions.* -[FluentAssertions]FluentAssertions.*"'
  )
end


task :opencoverreportgenerate do
  sh(ReportGeneratorExe,
    '-reports:' + OpenCoverReportFilePath,
    '-targetdir:' + File.join(OutputDir, 'OpenCoverReport'),
    '-reporttypes:Html'
  )
end


def get_nunit_console_args()
  testDllPaths = FileList[File.join(SourceDir, '**', 'bin', BuildConfiguration, '*.UnitTest.dll')]
  testDllPathsUnique = []
  # Remove DLL duplicates by name of fileContents
  testDllPaths.each do |currentItem|
    currentDllFileName = File.basename(currentItem)
	
	doesDllAlreadyExistInList = false
	testDllPathsUnique.each do |currentItem2|
	  currentDllFileName2 = File.basename(currentItem2)
	  if currentDllFileName2 == currentDllFileName then
	    doesDllAlreadyExistInList = true
	  end
	end
	
	if not doesDllAlreadyExistInList then
      testDllPathsUnique.push(currentItem)
	end
  end
  
  concatedTestDlls = testDllPathsUnique.join(" ")  
  return [concatedTestDlls,
    '/labels',
    '/xml=' + File.join(OutputDir, 'UnitTestResults.xml'),
    '/framework=4.5.1',
	'/nologo',
	'/noshadow']
end



################################
#  Package
################################
task(:versionnuspec) do
  fileContents = File.read(NuSpecFilePath)
  assemblyVersion = get_assembly_version()
  puts assemblyVersion
  
  #puts "sdfsdf<version>blahM</version>der".gsub(/(<version>)(.*)(<\/version>)/, '\11.2332\3')
  #puts "Z_sdsd: sdsd".gsub(/^(Z_.*): .*/, '\1')
  
  replacementText = "\\1" + assemblyVersion + "\\3"
  newFileContents = fileContents.gsub(/(<version>)(.*)(<\/version>)/, replacementText)
  File.open(NuSpecFilePath, 'w') {|f| f.write(newFileContents) }
end

desc "Get the Assembly's version number"
def get_assembly_version()
   fileContents = File.read(CommonAssemblyInfoFilePath)

   assemblyVersion = fileContents.match(/^\[assembly: AssemblyVersion\("(.*)"\)/i).captures[0]
   return assemblyVersion
end


desc "Create the nuget package"
task(:nugetpackage) do
  sh(NuGetExe, 'pack', NuSpecFilePath, '-symbols', '-OutputDirectory', OutputDir)
end


################################
#  Dependencies
################################
task :clean => [:msbuildclean, :cleanoutput]
task :setup => [:createoutputdir, :packagerestore]
task :compile => :msbuildcompile
task :unittest => :nunitunittest
task :unittestcoverage => [:opencovernunitunittest, :opencoverreportgenerate]
task :package => [:createoutputdir, :compile, :nugetpackage]
task :nugetpackage => :versionnuspec

task :default => [:clean, :setup, :compile, :unittestcoverage, :package]